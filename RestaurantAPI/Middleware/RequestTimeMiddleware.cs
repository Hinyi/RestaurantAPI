﻿using System.Diagnostics;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware;

public class RequestTimeMiddleware : IMiddleware
{
    private readonly ILogger<RequestTimeMiddleware> _logger;
    private readonly Stopwatch _stopWatch;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _logger = logger;
        _stopWatch = new Stopwatch();
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _stopWatch.Start();

        await next.Invoke(context);

        _stopWatch.Stop();
        var timeTaken = _stopWatch.ElapsedMilliseconds;
        if (timeTaken / 1000 > 4)
        {
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {timeTaken} ms";
            _logger.LogInformation(message);
        }
    }
}