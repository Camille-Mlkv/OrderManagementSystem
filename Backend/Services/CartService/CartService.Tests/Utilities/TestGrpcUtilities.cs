﻿using Grpc.Core;

namespace CartService.Tests.Utilities
{
    public static class TestGrpcUtilities
    {
        public static AsyncUnaryCall<T> CreateAsyncUnaryCall<T>(T response) where T : class
        {
            return new AsyncUnaryCall<T>(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { }
            );
        }
    }
}
