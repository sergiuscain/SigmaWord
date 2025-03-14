﻿using Microsoft.EntityFrameworkCore;
using SigmaWord.Services;

namespace SigmaWord
{
    public partial class App : Application
    {
        private readonly DbService _dbService;
        public App(DbService dbService)
        {
            InitializeComponent();

            MainPage = new AppShell();
            _dbService = dbService;
        }
        protected override async void OnStart()
        {
            await _dbService.InitializeDatabaseAsync();
            await _dbService.InitializeStatisticsAsync(190);
        }
    }
}
