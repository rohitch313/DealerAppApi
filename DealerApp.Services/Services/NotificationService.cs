using AutoMapper;
using DealerApp.Data;
using DealerApp.Dtos.DTO;
using DealerApp.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealerApp.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _db;
        private

    readonly IMapper _mapper;

        public

    NotificationService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ResponseDto> GetNotificationSupportAsync()
        {
            try
            {
                var notifications = await _db.Notificationstbl.ToListAsync();

                if (notifications == null || !notifications.Any())
                {
                    return new ResponseDto
                    {
                        Success = true, // Set to true even if no data, just no content
                        Data = null
                    };
                }

                var notificationDto = _mapper.Map<IEnumerable<NotificationDTO>>(notifications);

                return new ResponseDto
                {
                    Success = true,
                    Data = notificationDto
                };
            }
            catch (Exception ex)
            {

                return new ResponseDto
                {
                    Success = false,
                    Message = $"Internal Server Error:{ex}",
                };
            }
        }
    }
}