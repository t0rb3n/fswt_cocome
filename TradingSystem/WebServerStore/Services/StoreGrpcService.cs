using Application;
using Application.Store;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Store.V1;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services;

public class StoreGrpcService : StoreService.StoreServiceBase
{
    private readonly ILogger<StoreGrpcService> _logger;
    private readonly ICashDeskConnector _storeApplication;

    public StoreGrpcService(ILogger<StoreGrpcService> logger, ICashDeskConnector storeApplication)
    {
        _logger = logger;
        _storeApplication = storeApplication;
    }
}