using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcademiaMvc.Models;
using AcademiaMvc.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AcademiaMvc.Controllers
{
    public class AlunosController : Controller
    {
        private readonly AcademiaDbContext _context;
        private readonly EquipamentoService _equipamentoService;

        public AlunosController(AcademiaDbContext context, EquipamentoService equipamentoService)
        {
            _context = context;
            _equipamentoService = equipamentoService;
        }

        // GET: Alunos
        public async Task<IActionResult> Index()
        {
            var alunos = _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria);
            return View(await alunos.ToListAsync());
        }

        // GET: Alunos/Create
        public IActionResult Create()
        {
            CarregarCombos();
            return View();
        }

        // POST: Alunos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Idade,Email,Cpf,EquipamentoId,CategoriaId")] Aluno aluno)
        {
            if (_context.Alunos.Any(a => a.Cpf == aluno.Cpf))
            {
                ModelState.AddModelError("Cpf", "Já existe um aluno com este CPF.");
            }

            if (ModelState.IsValid)
            {
                if (aluno.EquipamentoId.HasValue)
                {
                    var sucesso = await _equipamentoService.AlocarEquipamento(aluno.EquipamentoId.Value);
                    if (!sucesso)
                    {
                        ModelState.AddModelError("EquipamentoId", "Este equipamento não está disponível.");
                        CarregarCombos(aluno);
                        return View(aluno);
                    }
                }

                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CarregarCombos(aluno);
            return View(aluno);
        }

        // GET: Alunos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return NotFound();

            CarregarCombos(aluno);
            return View(aluno);
        }

        // POST: Alunos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Idade,Email,Cpf,EquipamentoId,CategoriaId")] Aluno aluno)
        {
            if (id != aluno.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Alunos.Any(e => e.Id == aluno.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            CarregarCombos(aluno);
            return View(aluno);
        }
        // GET: Alunos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // GET: Alunos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var aluno = await _context.Alunos
                .Include(a => a.Equipamento)
                .Include(a => a.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null) return NotFound();

            return View(aluno);
        }

        // POST: Alunos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                if (aluno.EquipamentoId.HasValue)
                {
                    await _equipamentoService.DevolverEquipamento(aluno.EquipamentoId.Value);
                }

                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void CarregarCombos(Aluno? aluno = null)
        {
            ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos, "Id", "Nome", aluno?.EquipamentoId);
            //ViewData["EquipamentoId"] = new SelectList(_context.Equipamentos.Where(e => e.Quantidade > 0), "Id", "Nome", aluno?.EquipamentoId);
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "Id", "Nome", aluno?.CategoriaId);
        }
    }
}
