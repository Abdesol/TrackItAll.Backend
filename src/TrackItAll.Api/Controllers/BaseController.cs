using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TrackItAll.Application.Interfaces;
using TrackItAll.Domain.Entities;
using TrackItAll.Infrastructure.Services;

namespace TrackItAll.Api.Controllers;

/// <summary>
/// A controller that is inherited by other controllers to make use of the internal methods provided globally for several operations on the endpoint
/// </summary>
public class BaseController
    : ControllerBase
{
    private readonly IAzureAdTokenService _azureAdTokenService;
    private readonly IExpenseService _expenseService;

    public BaseController(IAzureAdTokenService azureAdTokenService, IExpenseService expenseService)
    {
        _azureAdTokenService = azureAdTokenService;
        _expenseService = expenseService;
    }

    /// <summary>
    /// A method used to verify an expense id with requirements like it needs to belong to the user requesting it
    /// </summary>
    /// <param name="claimsPrincipal">the user identity claims principal</param>
    /// <param name="id">id of the expense</param>
    /// <returns>returns a tupe of true value if successful, output result to return if not successful, the expense object and the owner id for indexing in the database </returns>
    protected async Task<(bool isSuccess, ObjectResult? result, Expense? expense, string ownerId)> VerifyExpense(
        ClaimsPrincipal claimsPrincipal, string id)
    {
        var oid = await _azureAdTokenService.GetUserObjectId(claimsPrincipal);
        var userRoles = claimsPrincipal.Claims.Select(c => c.Type);

        if (oid is null)
            return (false, Unauthorized("The user is not authenticated"), null, null)!;

        var expense = await _expenseService.GetExpense(id, oid);
        if (expense is null)
            return (false, BadRequest("The id of the expense is not found in the database"), null, oid);

        return expense.OwnerId == oid || userRoles.Contains("Admin")
            ? (true, null, expense, oid)
            : (false, Unauthorized("You are not the owner of this expense"), null, oid);
    }
}