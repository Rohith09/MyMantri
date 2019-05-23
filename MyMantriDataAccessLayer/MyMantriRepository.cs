using MyMantriDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MyMantriDataAccessLayer
{
    public class MyMantriRepository
    {
        public readonly MyMantriDBContext _context;

        public MyMantriRepository(MyMantriDBContext context)
        {
            _context = context;
        }

        public MyMantriRepository()
        {
            _context = new MyMantriDBContext();
        }

        public int ValidateUser(int userId, string password)
        {
            try
            {
                var user = _context.UserCredentials.Find(userId);
                if (user.Password == password)
                {
                    return user.RollId;
                }
            }
            catch (Exception ex)
            {
                return -99;
            }
            return -1;
        }

        public int ValidateMantri(string mantriId, string constituency)
        {
            int status = -1;
            try
            {
                Constituency _constituency = _context.Constituency.Find(constituency);
                if (_constituency.MantriUid == mantriId)
                {
                    if (_constituency.Status == "registered")
                    {
                        return 2;
                    }
                    _constituency.Status = "registered";
                    _context.SaveChanges();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                status = -1;
            }
            return status;
        }

        public int AddUserCredentials(string password, int rollId)
        {
            int userId = -1;
            try
            {
                UserCredentials user = new UserCredentials();
                user.Password = password;
                user.RollId = rollId;
                _context.UserCredentials.Add(user);
                _context.SaveChanges();
                userId = _context.UserCredentials.
                         Where(x => x.Password.Equals(password)).
                         Select(x => x.UserId).LastOrDefault();
            }
            catch (Exception ex)
            {
                return -99;
            }
            return userId;
        }

        public bool AddMantri(Mantri mantri)
        {
            bool status = false;
            try
            {
                _context.Mantri.Add(mantri);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool AddVoter(Voters voters)
        {
            bool status = false;
            try
            {
                _context.Voters.Add(voters);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public List<Mantri> ViewMantriForAdmin()
        {
            try
            {
                return _context.Mantri.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Mantri ViewMantriDetails(int mantriId)
        {
            try
            {
                return _context.Mantri.Find(mantriId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteMantri(int mantriId)
        {
            try
            {
                Mantri mantri = _context.Mantri.Find(mantriId);
                List<Workdone> workdone = _context.Workdone.Where(p => p.MantriId == mantri.MantriId).ToList();
                Constituency constituency = _context.Constituency.Where(p => p.ConstituencyName == mantri.Constituency).FirstOrDefault();
                constituency.Status = "not registered";
                List<Complaint> complaints = _context.Complaint.Where(p => p.Constituency == mantri.Constituency).ToList();
                List<Feedback> feedbacks = new List<Feedback>();
                foreach (var item in complaints)
                {
                    Feedback feedbackitem = _context.Feedback.Where(p => p.ComplaintId == item.ComplaintId).FirstOrDefault();
                    feedbacks.Add(feedbackitem);
                }
                UserCredentials userCredentials = _context.UserCredentials.Find(mantriId);
                _context.Feedback.RemoveRange(feedbacks);
                _context.Complaint.RemoveRange(complaints);
                _context.Workdone.RemoveRange(workdone);
                _context.Mantri.Remove(mantri);
                _context.UserCredentials.Remove(userCredentials);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdateMantri(Mantri mantri)
        {
            try
            {
                Mantri man = _context.Mantri.Find(mantri.MantriId);
                man.Address = mantri.Address;
                man.Constituency = mantri.Constituency;
                man.DateOfBirth = mantri.DateOfBirth;
                man.EmailId = mantri.EmailId;
                man.Gender = mantri.Gender;
                man.MantriId = mantri.MantriId;
                man.MantriUid = mantri.MantriUid;
                man.Mobile = mantri.Mobile;
                man.Name = mantri.Name;
                man.SecurityAns = mantri.SecurityAns;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetComplaintStatus(int complaintId)
        {
            try
            {
                if (complaintId == 0)
                {
                    return "X";
                }
                var cs = _context.Complaint.Find(complaintId);
                return cs.Status;
            }
            catch
            {
                return "W";
            }
        }

        public List<Voters> ViewVoterForAdmin()
        {
            try
            {
                return _context.Voters.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdateVoter(Voters voter)
        {
            try
            {
                Voters vot = _context.Voters.Find(voter.VoterId);
                vot.Address = voter.Address;
                vot.Complaint = voter.Complaint;
                vot.Constituency = voter.Constituency;
                vot.DateOfBirth = voter.DateOfBirth;
                vot.EmailId = voter.EmailId;
                vot.Gender = voter.Gender;
                vot.Mobile = voter.Mobile;
                vot.Name = voter.Name;
                vot.SecurityAns = voter.SecurityAns;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Voters ViewVoterDetails(int voterId)
        {
            try
            {
                return _context.Voters.Find(voterId);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public bool DeleteVoter(int voterId)
        {
            try
            {
                Voters voters = _context.Voters.Find(voterId);
                UserCredentials userCredentials = _context.UserCredentials.Find(voterId);
                List<Complaint> complaints = _context.Complaint.Where(p => p.VoterId == voters.VoterId).ToList();
                _context.Complaint.RemoveRange(complaints);
                _context.Voters.Remove(voters);
                _context.UserCredentials.Remove(userCredentials);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string ViewAdminName(int adminId)
        {
            try
            {
                Admin admin = _context.Admin.Find(adminId);
                return admin.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ViewMantriName(int mantriId)
        {
            try
            {
                Mantri mantri = _context.Mantri.Find(mantriId);
                return mantri.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ForgotPassword(UserCredentials userCredentials)
        {
            bool status = false;
            try
            {
                UserCredentials user = _context.UserCredentials.Find(userCredentials.UserId);
                user.Password = userCredentials.Password;
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public string ViewVoterName(int voterId)
        {
            try
            {
                Voters voters = _context.Voters.Find(voterId);
                return voters.Name;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ViewVoterCons(int voterId)
        {
            try
            {
                Voters voters = _context.Voters.Find(voterId);
                return voters.Constituency;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public int SaveAddComplaint(Complaint complaint)
        {
            int userId = 0;
            try
            {
                _context.Complaint.Add(complaint);
                _context.SaveChanges();
                userId = _context.Complaint.
                          Where(x => x.VoterId.Equals(complaint.VoterId)).
                          Select(x => x.ComplaintId).LastOrDefault();
            }
            catch (Exception ex)
            {
                userId = 0;
            }
            return userId;
        }

        public List<Complaint> ViewAllComplaintForVoter(int voterId)
        {
            try
            {
                var categoriesList = (from comp in _context.Complaint
                                      orderby comp.ComplaintId
                                      descending
                                      where comp.VoterId == voterId
                                      select comp).ToList();
                return categoriesList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Complaint ComplaintDetails(int complaintId)
        {
            try
            {
                return _context.Complaint.Find(complaintId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteComplaint(int ID)
        {
            bool status = false;
            try
            {
                var temp = _context.Complaint.Find(ID);
                _context.Complaint.Remove(temp);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool ForgotPassword(string EmailID, string SecurityAns, string pwd)
        {
            bool status = false;
            try
            {
                Voters voter = null;
                voter = _context.Voters.Where(p => p.EmailId.Equals(EmailID.ToLower())).FirstOrDefault();
                Mantri mantri = null;
                mantri = _context.Mantri.Where(p => p.EmailId.Equals(EmailID.ToLower())).FirstOrDefault();
                if (voter != null)
                {
                    if (SecurityAns.ToLower() == voter.SecurityAns.ToLower())
                    {
                        UserCredentials UC = _context.UserCredentials.Find(voter.VoterId);
                        UC.Password = pwd;
                        _context.SaveChanges();
                        status = true;
                    }
                }
                else if (mantri != null)
                {
                    if (SecurityAns.ToLower() == mantri.SecurityAns.ToLower())
                    {
                        UserCredentials UC = _context.UserCredentials.Find(mantri.MantriId);
                        UC.Password = pwd;
                        _context.SaveChanges();
                        status = true;
                    }
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public bool AddFeedback(Feedback feedback)
        {
            bool status = false;
            try
            {
                _context.Feedback.Add(feedback);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        public Feedback FindFeedback(int complaintId)
        {
            Feedback feed = null;
            try
            {
                feed = _context.Feedback.Find(complaintId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return feed;
        }

        public List<Mantri> GetAllConstituency()
        {
            try
            {
                return _context.Mantri.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public List<Workdone> WorkDone(int Id)
        {
            try
            {
                List<Workdone> wd = new List<Workdone>();
                wd = (from w in _context.Workdone
                      orderby w.WorkId descending
                      where w.MantriId == Id
                      select w).ToList();
                return wd;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Constituency> ConstituencyName()
        {
            try
            {
                List<Constituency> cons = new List<Constituency>();
                cons = (from w in _context.Constituency
                        select w).ToList();

                return cons;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> ConstituencyNameRegistered()
        {
            try
            {
                List<Constituency> cons = new List<Constituency>();
                List<string> c = new List<string>();
                cons = (from w in _context.Constituency
                        where w.Status == "registered"
                        select w).ToList();

                foreach (var item in cons)
                {
                    c.Add(item.ConstituencyName);
                }
                return c;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Mantri GetMantriByConstituency(string ConstituencyName)
        {
            Mantri mantri = null;
            try
            {
                mantri = _context.Mantri.Where(p => p.Constituency == ConstituencyName).FirstOrDefault();
                return mantri;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddWorkdone(Workdone workdone)
        {
            bool status = false;
            try
            {
                _context.Add(workdone);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public string GetConstituency(int mantriId)
        {
            try
            {
                var temp = _context.Mantri.Find(mantriId);
                return temp.Constituency;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Complaint> ViewAllComplaintForMantri(string cons)
        {
            try
            {
                var temp =(from c in _context.Complaint orderby c.ComplaintId descending where c.Constituency==cons select c).ToList();
                return temp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

      




        public bool UpdateComplaintStatus(int complaintId)
        {
            bool status = false;
            try
            {
                var temp = _context.Complaint.Find(complaintId);
                temp.Status = "Y";
                _context.SaveChanges();
                status = true;

            }
            catch (Exception ex)
            {

                status = false;
            }
            return status;
        }
        
        // WorkDone Methods
        public bool DeleteWork(int id)
        {
            bool status = false;
            try
            {
                var temp = _context.Workdone.Find(id);
                _context.Remove(temp);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public Workdone WorkDetails(int id)
        {
            return _context.Workdone.Find(id);
        }

        public bool EditWork(Workdone w)
        {
            bool status = false;
            try
            {
                var temp = _context.Workdone.Find(w.WorkId);
                temp.WorkDesr = w.WorkDesr;
                temp.Title = w.Title;
                _context.SaveChanges();
                status = true;
                return status;
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public int? ValidateEmailId(string emailId)
        {
            int? userId = null;
            try
            {
                if (_context.Admin.Where(p => p.EmailId == emailId).FirstOrDefault() == null)
                {
                    if (_context.Mantri.Where(p => p.EmailId == emailId).FirstOrDefault() == null)
                    {
                        if (_context.Voters.Where(p => p.EmailId == emailId).FirstOrDefault() == null)
                        {
                            return userId;
                        }
                        else
                        {
                            userId = _context.Voters.Where(p => p.EmailId == emailId).Select(p => p.VoterId).FirstOrDefault();
                        }
                    }
                    else
                    {
                        userId = _context.Mantri.Where(p => p.EmailId == emailId).Select(p => p.MantriId).FirstOrDefault();
                    }
                }
                else
                {
                    userId = _context.Admin.Where(p => p.EmailId == emailId).Select(p => p.AdminId).FirstOrDefault();
                }
                return userId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool sendEMailThroughOUTLOOK(string email, string otl)
        {
            try
            {
                //Random ran = new Random();
                //ran.
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Hello,Your link to change password : " + otl +"\n. This link will expire after 4 minutes.";
                //Subject line
                oMsg.Subject = "Change Password";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(email);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
                return true;
            }//end of try block
            catch (Exception ex)
            {
                return false;
            }//end of catch
        }

        public bool verifyemail(string name, string email, string password)
        {
            try
            {
                //Random ran = new Random();
                //ran.
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                string str = "Hello "+name+ ", please go to the given link to continue registration process. https://localhost:44320/Registration/Index?n="+name+"&e="+email+"&p="+password+"&k=";
                string hash = MakeExpiryHash(email);
                oMsg.HTMLBody = str + hash;
                //Subject line
                oMsg.Subject = "Verify Email ID";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(email);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
                return true;
            }//end of try block
            catch (Exception ex)
            {
                return false;
            }//end of catch
        }

        public static string MakeExpiryHash(string secure)
        {
            const string salt = "my mantri secure key";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + secure);
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }

        // Updates Status to Expired 
        public bool UpadteStatusExpired()
        {
            bool status = false;
            try
            {
                var temp = _context.Complaint.ToList();
                foreach (var item in temp)
                {
                    if ((DateTime.Now - item.ComplaintDateTime).TotalDays > 10)
                    {

                        item.Status = "E";
                        _context.SaveChanges();
                        status = true;
                    }
                }

            }
            catch (Exception ex)
            {

                status = false;
            }
            return status;
        }

        

        public List<Complaint> ComplaintList(int voterid)
        {
            try
            {
                var temp =  (from c in _context.Complaint orderby c.ComplaintId descending where c.VoterId == voterid select c).ToList();
                List<Complaint> TempList = new List<Complaint>();
                foreach (var item in temp)
                {
                    if (item.VoterId == voterid)
                    {
                        TempList.Add(item);
                    }
                }
                return TempList;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public string UpdatePassword(int id, string op, string np)
        {
            string status = "e";
            try
            {
                var o = _context.UserCredentials.Find(id);
                if (o.Password == op)
                {
                    o.Password = np;
                    status = "t";
                    _context.SaveChanges();
                }

                else
                {
                    status = "f";
                }


            }
            catch (Exception e)
            {

                status = "e";
            }
            return status;
        }


        //When Status of Complaint updated to Solved
        public bool SolvedEmail(string email, int comaplintId)
        {
            try
            {
                //Random ran = new Random();
                //ran.
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Dear User, <br> Your Compalint with ComaplintID: " + comaplintId + " has been resolved.<br> Please login and provide the feedback for the same.";
                //Subject line
                oMsg.Subject = "Your Compalint has been Resolved!!";
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(email);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
                return true;
            }//end of try block
            catch (Exception ex)
            {
                return false;
            }//end of catch
        }




        public bool complaintEMail(string Email, string UserName, int complaintId, DateTime dateTime, string Category, string cons, string desr)
        {
            try
            {
                //Random ran = new Random();
                //ran.
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = "Dear " + UserName + ",<br><br>" +
                    "Your complaint has been successfully registrered with <br> Complaint ID: " + complaintId +
                    " <br> Category: " + Category + "<br> Description: " + desr + " <br>Time " + dateTime;

                //Subject line
                oMsg.Subject = "Your Complaint has been successfully registered with Complaint Id: " + complaintId;
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(Email);
                oRecip.Resolve();
                // Send.
                oMsg.Send();
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
                return true;
            }//end of try block
            catch (Exception ex)
            {
                return false;
            }//end of catch
        }

    }
}
