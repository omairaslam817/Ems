﻿using Ems.Domain.Shared;
using MediatR;

namespace Ems.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
