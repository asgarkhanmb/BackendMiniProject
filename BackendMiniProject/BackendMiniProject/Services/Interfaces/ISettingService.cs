﻿namespace BackendMiniProject.Services.Interfaces
{
    public interface ISettingService 
    {
        Task<Dictionary<string, string>> GetAllAsync();
    }
}