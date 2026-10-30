using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;

namespace ShootingGame
{
     public record ScoreData(int Rank, string Name, int Score);

    internal class ScoreNetworkManager
    {

        // 通信本体, static only で使いまわす.
        private static readonly HttpClient client = new HttpClient()
        {
            //サーバーが起動していない時にゲームが固まらないよう、2秒で諦める設定にする
            Timeout = TimeSpan.FromSeconds(2.0)
        };

<<<<<<< HEAD
        // PC2側は"http://192.168.0.10:5000/score"とかにする? ipconfigするのを忘れない.
=======
        // PC2側は"http://192.168.10.2:5000/score"とかにする? ipconfigするのを忘れない.
>>>>>>> 8cae0bd435b6d017a4e60c79d337d4ed363d8072
        private string serverUrl = "http://192.168.10.1:5000/score";

        // 通信成功ならtrue , 失敗ならfalseを返す. 例外は投げない.
        public bool SendScore(int rank, string name, int score)
        {
            try
            {
               ScoreData data = new ScoreData(rank, name, score);
                // サーバーにデータ送信. 完了まで待つ.
                HttpResponseMessage response = client.PostAsJsonAsync(serverUrl, data).GetAwaiter().GetResult();
                // サーバーが「OK」なら true
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // サーバーが起動していない. LANが抜けているなどはこっち.
                return false;
            }
        }

        // 通信成功ならランキングリストを返す. 失敗ならnullを返す. 例外は投げない.
        public List<ScoreData> GetRanking()
        {
            try
            {
                // サーバーからデータ取得. 完了まで待つ.
                List<ScoreData> ranking = client.GetFromJsonAsync<List<ScoreData>>(serverUrl).GetAwaiter().GetResult();
                // ?? はnull合体演算子. nullなら空のリストを返す.
                return ranking ?? new List<ScoreData>();
            }
            catch (Exception ex)
            {
                // サーバーが起動していない. LANが抜けているなどではnull で知らせる.
                Debug.WriteLine("GET失敗の原因: " + ex.Message);
                return null;
            }
        }
    }
}
