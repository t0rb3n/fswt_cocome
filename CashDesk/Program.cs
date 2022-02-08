using CashDesk;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

CashDeskEventHandler cdeh = new CashDeskEventHandler();



app.Run();

