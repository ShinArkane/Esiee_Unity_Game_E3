namespace STUDENT_NAME
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections.Generic;
	using SDD.Events;
	using System.Linq;
    using System;
    using UnityEngine.SceneManagement;

    public enum GameState { gameMenu, gamePlay, gameNextLevel, gamePause, gameOver, gameVictory }

	public class GameManager : Manager<GameManager>
	{
		#region Game State
		private GameState m_GameState;
		public bool IsPlaying { get { return m_GameState == GameState.gamePlay; } }
		#endregion

		//LIVES
		#region Lives
		[Header("GameManager")]
		[SerializeField]
		private int m_NStartLives;

		private int m_NLives;
		public int NLives { get { return m_NLives; } }
		void DecrementNLives(int decrement)
		{
			SetNLives(m_NLives - decrement);
		}

		void SetNLives(int nLives)
		{
			m_NLives = nLives;
			EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eBestEnemyScore = BestEnemyScore, eScore = m_Score, eEnemyScore = m_EnemyScore, eNLives = m_NLives });
		}
		#endregion


		#region Score
		private float m_Score;
		private float m_EnemyScore;
		public float Score
		{
			get { return m_Score; }
			set
			{
				m_Score = value;
				BestScore = Mathf.Max(BestScore, value);
			}
		}

		public float EnemyScore
		{
			get { return m_EnemyScore; }
			set
			{
				m_EnemyScore = value;
				BestEnemyScore = Mathf.Max(BestEnemyScore, value);
			}
		}

		public float BestScore
		{
			get { return PlayerPrefs.GetFloat("BEST_SCORE", 0); }
			set { PlayerPrefs.SetFloat("BEST_SCORE", value); }
		}

		public float BestEnemyScore
		{
			get { return PlayerPrefs.GetFloat("BEST_SCORE_ENEMY", 0); }
			set { PlayerPrefs.SetFloat("BEST_SCORE_ENEMY", value); }
		}

		void IncrementScore(float increment)
		{
			SetScore(m_Score + increment);
		}

		void SetScore(float score, bool raiseEvent = true)
		{
			Score = score;

			if (raiseEvent)
				EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eBestEnemyScore = BestEnemyScore, eScore = m_Score, eEnemyScore = m_EnemyScore, eNLives = m_NLives });
		}

		private void IncrementEnemyScore(float increment)
		{
			SetEnemyScore(m_EnemyScore + increment);
		}

		private void SetEnemyScore(float enemyScore, bool raiseEvent = true)
        {
			EnemyScore = enemyScore;

			if (raiseEvent)
				EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eBestScore = BestScore, eBestEnemyScore = BestEnemyScore, eScore = m_Score, eEnemyScore = m_EnemyScore, eNLives = m_NLives });
		}
		#endregion

		#region Time
		void SetTimeScale(float newTimeScale)
		{
			Time.timeScale = newTimeScale;
		}
		#endregion


		#region Events' subscription
		public override void SubscribeEvents()
		{
			base.SubscribeEvents();

			//GameEvent
			EventManager.Instance.AddListener<GameVictoryEvent>(LevelHasBeenWin);
			EventManager.Instance.RemoveListener<GameOverEvent>(LevelHasBeenLose);

			//MainMenuManager
			EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);

			EventManager.Instance.AddListener<PlayLevel1ButtonClickedEvent>(PlayLevel1ButtonClicked);

			//Score
			EventManager.Instance.AddListener<ScoreItemEvent>(ScoreHasBeenGained);
			EventManager.Instance.AddListener<ScoreEnemyEvent>(EnemyScoreHasBeenGained);

			//Life
			EventManager.Instance.AddListener<LifeEvent>(LifeHasBeenModified);
		}

        

        public override void UnsubscribeEvents()
		{
			base.UnsubscribeEvents();

			EventManager.Instance.RemoveListener<GameVictoryEvent>(LevelHasBeenWin);
			EventManager.Instance.RemoveListener<GameOverEvent>(LevelHasBeenLose);


			//MainMenuManager
			EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
			EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
			EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
			EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
			EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);

			EventManager.Instance.RemoveListener<PlayLevel1ButtonClickedEvent>(PlayLevel1ButtonClicked);

			//Score
			EventManager.Instance.RemoveListener<ScoreItemEvent>(ScoreHasBeenGained);
			EventManager.Instance.RemoveListener<ScoreEnemyEvent>(EnemyScoreHasBeenGained);

			//Life
			EventManager.Instance.RemoveListener<LifeEvent>(LifeHasBeenModified);
		}
        #endregion

        #region Manager implementation
        protected override IEnumerator InitCoroutine()
		{
			Menu();
			InitNewGame(); // essentiellement pour que les statistiques du jeu soient mise à jour en HUD
			yield break;
		}
		#endregion

		#region Game flow & Gameplay
		//Game initialization
		void InitNewGame(bool raiseStatsEvent = true)
		{
			SetNLives(5);
			SetScore(0);
			SetEnemyScore(0);
		}


		private void LifeHasBeenModified(LifeEvent e)
		{
			DecrementNLives(e.eLife);
		}

		private void LevelHasBeenWin(GameVictoryEvent e)
		{
			Victory();
		}

		private void LevelHasBeenLose(GameOverEvent e)
		{
			Over();
		}
		#endregion

		#region Callbacks to events issued by Score items
		private void ScoreHasBeenGained(ScoreItemEvent e)
		{
			if (IsPlaying)
				IncrementScore(e.eScore);
		}

		private void EnemyScoreHasBeenGained(ScoreEnemyEvent e)
		{
			if (IsPlaying)

				IncrementEnemyScore(e.eScore);
		}
		#endregion





		#region Callbacks to Events issued by MenuManager
		private void PlayLevel1ButtonClicked(PlayLevel1ButtonClickedEvent e) {
			launchLevel1();
		}

        private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			//Menu();
		}

		private void PlayButtonClicked(PlayButtonClickedEvent e)
		{
			Play();
		}

		private void ResumeButtonClicked(ResumeButtonClickedEvent e)
		{
			Resume();
		}

		private void EscapeButtonClicked(EscapeButtonClickedEvent e)
		{
			if (IsPlaying) Pause();
		}

		private void QuitButtonClicked(QuitButtonClickedEvent e)
		{
			Application.Quit();
		}
		#endregion

		#region GameState methods
		private void Menu()
		{
			SetTimeScale(1);
			m_GameState = GameState.gameMenu;
			
			if(MusicLoopsManager.Instance)MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
			EventManager.Instance.Raise(new GameMenuEvent());
		}

		private void Play()
		{
			InitNewGame();
			SetTimeScale(1);
			m_GameState = GameState.gamePlay;

			if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
			EventManager.Instance.Raise(new GamePlayEvent());
		}

		private void launchLevel1()
		{
			// poser le joueur sur le level 1
			InitCoroutine();
			InitNewGame();
			SetTimeScale(1);
			m_GameState = GameState.gamePlay;
			if (MusicLoopsManager.Instance) MusicLoopsManager.Instance.PlayMusic(Constants.GAMEPLAY_MUSIC);
			EventManager.Instance.Raise(new LaunchLevel1Event());
			EventManager.Instance.Raise(new GamePlayEvent());
			Play();

		}

		private void Pause()
		{
			if (!IsPlaying) return;

			SetTimeScale(0);
			m_GameState = GameState.gamePause;
			EventManager.Instance.Raise(new GamePauseEvent());
		}

		private void Resume()
		{
			if (IsPlaying) return;

			SetTimeScale(1);
			m_GameState = GameState.gamePlay;
			EventManager.Instance.Raise(new GameResumeEvent());
		}

		private void Over()
		{
			SetTimeScale(0);
			m_GameState = GameState.gameOver;
			EventManager.Instance.Raise(new GameOverEvent());
			if(SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEOVER_SFX);
		}

		private void Victory()
		{
			m_GameState = GameState.gameVictory;
			if (SfxManager.Instance) SfxManager.Instance.PlaySfx2D(Constants.GAMEVICTORY_SFX);
		}
		#endregion

	}
}

