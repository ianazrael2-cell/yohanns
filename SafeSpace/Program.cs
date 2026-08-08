using SafeSpace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddScoped<IdentityVerificationService>();
builder.Services.AddScoped<AvailabilityService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ChannelSyncService>();
builder.Services.AddScoped<StaySureWorkflowService>();

var app = builder.Build();

app.UseStaticFiles();
app.MapRazorPages();
app.MapControllers();

app.Run();
