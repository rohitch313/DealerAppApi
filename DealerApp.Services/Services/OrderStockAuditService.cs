using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Model.Models;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Services
{
    public class OrderStockAuditService : IOrderStockAuditService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderStockAuditService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// This is used   to get Stock order list
        /// </summary>

        public async Task<IEnumerable<Order_StockAuditDTO>> GetStockSupportAsync()
        {
            try
            {
                IEnumerable<Order_StockAudit> stockAuditData = await _context.Order_StockAuditstbl.ToListAsync();

                if (!stockAuditData.Any())
                {
                    return null; // Indicate no data found
                }

                return _mapper.Map<IEnumerable<Order_StockAuditDTO>>(stockAuditData);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.Error.WriteLine(ex.Message);
                throw; // Rethrow for handling in the controller
            }
        }

        /// <summary>
        /// This is used   to Post Stock order 
        /// </summary>
        public async Task<Order_StockAuditDTO> PostStockSupportAsync(Order_StockAuditDTO stockAuditDTO)
        {
            try
            {
                if (stockAuditDTO == null)
                {
                    throw new ArgumentNullException(nameof(stockAuditDTO));
                }

                // Optional: Validation if an item with the same Id already exists
                // if (_context.Order_StockAuditstbl.Any(v => v.Id == stockAuditDTO.Id))
                // {
                //     throw new InvalidOperationException("Item with the same Id already exists");
                // }

                var stockAuditModel = _mapper.Map<Order_StockAudit>(stockAuditDTO);

                _context.Order_StockAuditstbl.Add(stockAuditModel);
                await _context.SaveChangesAsync();

                return _mapper.Map<Order_StockAuditDTO>(stockAuditModel);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.Error.WriteLine(ex.Message);
                throw; // Rethrow for handling in the controller
            }
        }
    }
}