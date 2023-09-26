using Microsoft.OpenApi.Models;
using BokhandelensRESTApi.DATA;
using BokhandelensRESTApi.Repository;
using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BokhandelensRESTApi.Endpoints;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        app.MapGet("/books", GetBooks).WithName("GetBooks").WithOpenApi();
        
        app.MapGet("/books/{id}", GetbyId).WithName("GetById").WithOpenApi();
        
        app.MapPost("/books", AddBook).WithName("AddBook").WithOpenApi();
        
        app.MapPut("/books/{id}", UpdateBook).WithName("UpdateBook").WithOpenApi();
        
        app.MapDelete("/books/{id}", DeleteBook).WithName("DeleteBook").WithOpenApi();
   
    }



    // ENDPOINTS
    private static IResult GetBooks(IBookRepository repo, ILogger<Program> logger, [FromQuery] string? title, [FromQuery] string? author, [FromQuery] int? publicationYear)
    {
        // FILTERS
        if (title != null)
        {
            return Results.Ok(repo.GetAll().Where(b => b.Title == title));
        }
        if (author != null)
        {
            return Results.Ok(repo.GetAll().Where(b => b.Author == author));
        }

        if (publicationYear != null)
        {
            return Results.Ok(repo.GetAll().Where(b => b.PublicationYear == publicationYear));
        }

        return Results.Ok(repo.GetAll());
    }

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
        var bookToDelete = repo.Delete(id);
        if (bookToDelete == null)
            return Results.NotFound($"Book with id:{id} does not exist");
        return Results.Ok(bookToDelete);
    }
    
    private static IResult UpdateBook(IBookRepository repo, int id, Book book)
    {
        var updatedBook = repo.Update(id, book);
        if (updatedBook == null)
            return Results.NotFound($"Cant find book to update with id:{id}");

        return Results.Ok(updatedBook);

    }

   




}
