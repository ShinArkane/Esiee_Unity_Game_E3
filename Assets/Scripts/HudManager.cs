namespace STUDENT_NAME
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	using SDD.Events;

	public class HudManager : Manager<HudManager>
	{
		[Header("Score")]
		[SerializeField] Text labelLife;
		[SerializeField] Text Life;

		[Header("Score")]
		#region Labels & Values
		[SerializeField] Text labelScoreCoin;
		[SerializeField] Text ScoreCoin;
		[SerializeField] Text labelScoreEnemy;
		[SerializeField] Text ScoreEnemy;
		//[SerializeField] Text labelScoreTime;
		//[SerializeField] Text ScoreTime;
		[Header("BestScore")]
		[SerializeField] Text labelBestScoreCoin;
		[SerializeField] Text ScoreBestCoin;
		[SerializeField] Text labelBestScoreEnemy;
		[SerializeField] Text ScoreBestEnemy;
		//[SerializeField] Text labelBestScoreTime;
		//[SerializeField] Text ScoreBestTime;
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
		#endregion

		#region Callbacks to GameManager events
		protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
			Life.text = e.eNLives.ToString();

			ScoreCoin.text = e.eScore.ToString();
			ScoreBestCoin.text = e.eBestScore.ToString();

			ScoreEnemy.text = e.eEnemyScore.ToString();
			ScoreBestEnemy.text = e.eBestEnemyScore.ToString();


		}
		#endregion

	}
}