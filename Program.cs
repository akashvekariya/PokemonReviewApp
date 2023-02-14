using Microsoft.EntityFrameworkCore;
using PokemonReviewApp;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;

var builder = WebApplication.CreateBuilder(args); {
	builder.Services.AddControllers();

	// to add the seed data
	builder.Services.AddTransient<Seed>();

	// to add the automapper
	builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen();

	// to add the database and context
	builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

	// to add the repository | dependency injection
	builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
	builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
	builder.Services.AddScoped<ICountryRepository, CountryRepository>();
	builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
	builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
	builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();

}

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); {
	if (args.Length == 1 && string.Equals(args[0], "seeddata", StringComparison.OrdinalIgnoreCase))
		SeedData(app);

	void SeedData(IHost app) {
		var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
		using var scope = scopedFactory.CreateScope();
		var service = scope.ServiceProvider.GetService<Seed>();
		service.SeedDataContext();
	}
}

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
