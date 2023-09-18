using Microsoft.OpenApi.Models;
using BokhandelensRESTApi.DATA;
using BokhandelensRESTApi.Repository;
using System;
using System.Runtime.CompilerServices;

namespace BokhandelensRESTApi.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", GetBooks).WithName("GetBooks").WithOpenApi();
        /*
        app.MapGet("/books/{id}", GetbyId).WithName("GetById").WithOpenApi();

        app.MapPost("/books", AddBook).WithName("AddBook").WithOpenApi();

        app.MapPut("/books/{id}", UpdateBook).WithName("UpdateBook").WithOpenApi();

        app.MapDelete("/books/{id}", DeleteBook).WithName("DeleteBook").WithOpenApi();
        */
    }


    private static IResult GetBooks(IBookRepository repo, ILogger<Program> logger)
    {
        return Results.Ok(repo.GetAll());
    }


    /*
    private static IResult GetbyId(IBookRepository repo, int id)
    {
        if (repo == null)
            return Results.NoContent();

        var p = repo.GetById(id);
        if (p != null)
            return Results.Ok(p);
        return Results.NotFound($"Could not find book with id:{id}");

    }

    private static IResult AddBook(IBookRepository repo, Book book)
    {

        if (book != null)
        {
            var b = repo.Add(book);
            if (b != null)
                return Results.Ok(b);
        }
        return Results.NoContent();

    }

    private static IResult DeleteBook(IBookRepository repo, int id)
    {
        var PersonToDelete = repo.Delete(id);
        if (PersonToDelete == null)
            return Results.NotFound("Person with that Id does not exist");
        return Results.Ok(PersonToDelete);
    }

    private static IResult UpdateBook(IBookRepository repo, int id, Book book)
    {
        var personToUpdate = repo.Update(id, Book);
        if (personToUpdate == null)
            return Results.NotFound($"Cant find person to update with id:{id}");

        return Results.Ok(personToUpdate);

    }

   */




}
