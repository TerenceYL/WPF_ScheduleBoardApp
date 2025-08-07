using System;

namespace ScheduleBoardApp.Models
{
    /// <summary>
    /// 代表一筆排程任務（看板卡片）
    /// </summary>
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Lane { get; set; } = string.Empty;   // 泳道名稱
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}