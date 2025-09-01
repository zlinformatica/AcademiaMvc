using AcademiaMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademiaMvc.Services
{
    public class EquipamentoService
    {
        private readonly AcademiaDbContext _context;

        public EquipamentoService(AcademiaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AlocarEquipamento(int equipamentoId)
        {
            var equipamento = await _context.Equipamentos.FindAsync(equipamentoId);
            if (equipamento == null || equipamento.Quantidade <= 0)
                return false;

            equipamento.Quantidade--;
            _context.Update(equipamento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DevolverEquipamento(int equipamentoId)
        {
            var equipamento = await _context.Equipamentos.FindAsync(equipamentoId);
            if (equipamento != null)
            {
                equipamento.Quantidade++;
                _context.Update(equipamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
