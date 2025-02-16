using Microsoft.EntityFrameworkCore;
using SigmaWord.Services;

namespace SigmaWord
{
    public partial class App : Application
    {
        private readonly SigmaWordDbContext _context;
        public App(SigmaWordDbContext context)
        {
            InitializeComponent();

            MainPage = new AppShell();
            _context = context;

        }
        protected override void OnStart()
        {
            _context.Database.Migrate();
        }
    }
}
