using MyMantriDataAccessLayer.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMantriDataAccessLayer
{
    public class StipulatedTimeForComplaints : IJob
    {
        public readonly MyMantriDBContext _context;

        public StipulatedTimeForComplaints(MyMantriDBContext context)
        {
            _context = context;
        }

        public StipulatedTimeForComplaints()
        {
            _context = new MyMantriDBContext();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var temp = _context.Complaint.ToList();
                foreach (var item in temp)
                {
                    if (item.Category.ToLower() == "water")
                    {
                        if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 5)
                        {
                            item.Status = "E";
                            _context.SaveChanges();
                        }
                    }
                    if (item.Category.ToLower() == "electricity")
                    {
                        if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 5)
                        {
                            item.Status = "E";
                            _context.SaveChanges();
                        }
                    }
                    if (item.Category.ToLower() == "sanitation")
                    {
                        if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 10)
                        {
                            item.Status = "E";
                            _context.SaveChanges();
                        }
                    }
                    if (item.Category.ToLower() == "road")
                    {
                        if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 15)
                        {
                            item.Status = "E";
                            _context.SaveChanges();
                        }
                    }
                    if (item.Category.ToLower() == "other")
                    {
                        if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 15)
                        {
                            item.Status = "E";
                            _context.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return null;
        }
    }
}
