using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ScheduleBoardApp.Models;

namespace ScheduleBoardApp.ViewModels
{
    /// <summary>
    /// 主要的畫面 ViewModel
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        // 所有泳道（自動根據 JSON 中的 Lane 欄位產生）
        public ObservableCollection<LaneViewModel> Lanes { get; } = new();

        // 整體任務清單（若有需要直接存取可以使用此屬性）
        public ObservableCollection<TaskItem> AllTasks { get; } = new();

        public MainViewModel()
        {
            LoadTasksFromJson();
            BuildLanes();
        }

        private void LoadTasksFromJson()
        {
            try
            {
                // 假設 tasks.json 放在專案根目錄的 data 資料夾
                var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "tasks.json");
                if (!File.Exists(jsonPath))
                {
                    // 若檔案不存在，寫入範例資料（方便第一次執行）
                    File.WriteAllText(jsonPath, SampleJson);
                }

                var json = File.ReadAllText(jsonPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json, options) ?? new List<TaskItem>();

                foreach (var t in tasks)
                    AllTasks.Add(t);
            }
            catch (Exception ex)
            {
                // 真實專案可改成寫入 Log 檔或顯示訊息框
                System.Diagnostics.Debug.WriteLine($"載入 JSON 失敗: {ex.Message}");
            }
        }

        private void BuildLanes()
        {
            // 依照 Lane 欄位去除重複、排序
            var distinctLanes = AllTasks
                .Select(t => t.Lane)
                .Distinct()
                .OrderBy(l => l); // 如需自訂順序，可改為自訂排序表

            foreach (var laneName in distinctLanes)
            {
                var lane = new LaneViewModel { Name = laneName };
                var tasksInLane = AllTasks.Where(t => t.Lane == laneName);
                foreach (var task in tasksInLane)
                    lane.Tasks.Add(task);
                Lanes.Add(lane);
            }
        }

        // ------------------- 範例 JSON（首次執行時自動寫入） -------------------
        private const string SampleJson = """
        [
            {
                "Id": 1,
                "Title": "需求分析",
                "Owner": "Alice",
                "Lane": "待處理",
                "StartDate": "2025-08-10T00:00:00",
                "EndDate": "2025-08-12T00:00:00"
            },
            {
                "Id": 2,
                "Title": "UI 設計",
                "Owner": "Bob",
                "Lane": "進行中",
                "StartDate": "2025-08-11T00:00:00",
                "EndDate": "2025-08-15T00:00:00"
            },
            {
                "Id": 3,
                "Title": "程式實作",
                "Owner": "Charlie",
                "Lane": "待處理",
                "StartDate": "2025-08-12T00:00:00",
                "EndDate": "2025-08-20T00:00:00"
            },
            {
                "Id": 4,
                "Title": "單元測試",
                "Owner": "Alice",
                "Lane": "完成",
                "StartDate": "2025-08-13T00:00:00",
                "EndDate": "2025-08-14T00:00:00"
            },
            {
                "Id": 5,
                "Title": "整合測試",
                "Owner": "Bob",
                "Lane": "完成",
                "StartDate": "2025-08-15T00:00:00",
                "EndDate": "2025-08-18T00:00:00"
            }
        ]
        """;
        // ---------------------------------------------------------------------

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}