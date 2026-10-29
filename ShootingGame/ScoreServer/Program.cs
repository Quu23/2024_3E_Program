using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string filePath = "scoreboard.txt";

// 同時アクセスによるファイル読み書きエラーを防ぐためのロック
SemaphoreSlim fileLock = new SemaphoreSlim(1, 1);

// 【GET】ランキングデータをそのまま返す（手動RELOAD用）
app.MapGet("/", async () =>
{
    await fileLock.WaitAsync();
    try
    {
        if (!File.Exists(filePath))
        {
            return Results.NotFound("Scoreboard file not found.");
        }

        var textContent = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
        return Results.Text(textContent, contentType: "text/plain; charset=utf-8");
    }
    finally
    {
        fileLock.Release();
    }
});

// 【POST】ゲームオーバー/クリア時に名前とスコアを受け取り、マージ・ソートして保存
app.MapPost("/score", async (ScoreRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Name) || request.Score < 0)
    {
        return Results.BadRequest("Invalid name or score.");
    }

    await fileLock.WaitAsync();
    try
    {
        var scoreList = new List<(int Score, string Name)>();

        // 既存ファイルの読み込みとパース
        if (File.Exists(filePath))
        {
            var lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                // 「順位,スコア,名前」の形式からスコアと名前を取り出す
                if (parts.Length >= 3 && int.TryParse(parts[1], out int score))
                {
                    scoreList.Add((score, parts[2]));
                }
            }
        }

        // 送信された新しいスコアを追加
        scoreList.Add((request.Score, request.Name));

        // スコアが高い順（降順）に並び替え
        var sortedList = scoreList
            .OrderByDescending(x => x.Score)
            .ToList();

        // 順位を振り直してテキスト整形（順位,スコア,名前）
        var sb = new StringBuilder();
        for (int i = 0; i < sortedList.Count; i++)
        {
            int rank = i + 1;
            sb.AppendLine($"{rank},{sortedList[i].Score},{sortedList[i].Name}");
        }

        // ファイルへ保存
        await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);

        return Results.Ok("Score registered.");
    }
    finally
    {
        fileLock.Release();
    }
});

// LAN内の他PCからアクセスできるよう "http://0.0.0.0:5000" に変更
app.Run("http://0.0.0.0:5000");

// POSTリクエスト用のデータモデル
public record ScoreData(int Rank, string Name, int Score);