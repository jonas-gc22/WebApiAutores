﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
	[ApiController]
	[Route("api/libros")]
	public class LibrosController : ControllerBase
	{
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
		{
			this.context = context;
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Libro>> Get(int id)
		{
			return await context.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);
		}

		[HttpPost]
		public async Task<ActionResult> Post(Libro libro)
		{
			var existsAuthor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

			if (!existsAuthor)
			{
				return BadRequest($"Not exists the author ID: {libro.AutorId}");
			}

			context.Add(libro);
			await context.SaveChangesAsync();
			return Ok();
		}
	}
}

