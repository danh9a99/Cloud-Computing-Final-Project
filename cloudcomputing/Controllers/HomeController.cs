
using cloudcomputing.Efs.Context;
using cloudcomputing.Efs.Entities;
using cloudcomputing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;
        SshClient sshclient = new SshClient("192.168.1.116", "httql189", "951350");
        public string nameTemp = "";
        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("/trang-chu")]
        public IActionResult Index()
        {
            //Lấy nội dung bảng container theo UserID
            var _userID = _context.Users.Select(s => new
            {
                userID = s.UserID,
                userName = s.UserName

            }).ToList();
            var _containerTable = _context.Containers.ToList();
            
            sshclient.Connect();
            SshCommand sc = sshclient.CreateCommand("docker ps --format \"table{{.Names}}\"");
            sc.Execute();

           
            ViewBag.ContainerNameStatus = sc.Result;
            sshclient.Disconnect();
            ViewBag.UserID = JsonConvert.SerializeObject(_userID);
            ViewBag.Containers = JsonConvert.SerializeObject(_containerTable);
            return View();

        }
        [HttpPost]
        public IActionResult Index(Command command)
        {
            string message = "";
            //SshCommand sc = sshclient.CreateCommand("docker run hello-world");
            //sc.Execute();
            //string answer = sc.Result;
            if (ModelState.IsValid)
            {


                //SshClient sshclientDocker = new SshClient(command.ip, command.serverName, command.passWord);
                sshclient.Connect();

                ////sshclient.Connect();

                //message = sc.Result;
                return Redirect("home/getCommand");
            }
            else
            {
                message = "Failed to create the product. Please try again";
            }

            return Content(message);
        }
        [HttpGet]
        public IActionResult getCommand()
        {

            return View();
        }
        public IActionResult getCommand(DockerCmd dockerCmd)
        {
            //SshClient sshclient = new SshClient(_ip, _user, _pass);
            //sshclient.Connect();
            //SshCommand sc = sshclient.CreateCommand("docker run hello-world");
            //sc.Execute();
            //string answer = sc.Result;

            return View();
        }
        public async Task<IActionResult> Login()
        {
            List<LoginViewModel> listuser = new List<LoginViewModel>();
            listuser = await _context.Users.AsNoTracking()
                .Select(h => new LoginViewModel { Username = h.UserName, Password = h.PassWord })
                .ToListAsync();
            ViewBag.ListUser = JsonConvert.SerializeObject(listuser);
            //Kết nối SSH

            //
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContainerModel containerModel)
        {
            double count;
            if (ModelState.IsValid)
            {
                //Lấy containerID, userName từ database
                var _userID = _context.Users
                    .Where(c => c.UserName == containerModel.ContainerName)
                    .Select(c => c.UserID).FirstOrDefault();
                try
                {
                    count = _context.Containers
                    .Where(c => c.UserID == _userID)
                    .Count();
                }
                catch (Exception e)
                {
                    count = 0;
                }



                //add db
                _context.Containers.Add(new Container
                {
                    ContainerName = "Container_" + containerModel.ContainerName + "_" + (count + 1),
                    LimitCPU = containerModel.LimitCPU,
                    LimitRAM = containerModel.LimitRAM,
                    CreatedDate = DateTime.Now,
                    UserID = _userID

                });
                _context.SaveChanges();
                //Run docker
                sshclient.Connect();
                try
                {

                }
                catch (Exception e)
                {

                }

                string name = "Container_" + containerModel.ContainerName + "_" + (count + 1);
                SshCommand sc = sshclient.CreateCommand("docker run --memory=\"" + containerModel.LimitRAM + "m\" --cpus=\"" + containerModel.LimitCPU / 100 + "\" --name " + name + " -d -it ubuntu:18.04");
                if (sc.Error == "")
                {
                    sc.Execute();
                    sshclient.Disconnect();
                    //sshclient.CreateCommand("java -version").Execute();
                    //return Content(sshclient.CreateCommand("java -version").Result);
                }
                else
                {
                    return Content(sc.Error);
                }

            }
            return View("Index");
        }
        [HttpPost]
        public IActionResult StartUp([FromBody] ModelStartUpDocker input)
        {
            if (input == null)
            {
                return Content("Lỗi hệ thống");
            }

            //Run docker
            sshclient.Connect();
            SshCommand sc = sshclient.CreateCommand("docker start " + input.ContainerName);
            sc.Execute();
            sshclient.Disconnect();
            //
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult ShutDown([FromBody] ModelStartUpDocker input)
        {
            if (input == null)
            {
                return Content("Lỗi hệ thống");
            }

            //Run docker
            sshclient.Connect();
            SshCommand sc = sshclient.CreateCommand("docker stop " + input.ContainerName);
            sc.Execute();
            sshclient.Disconnect();
            //
            return RedirectToAction("Index", "Home");
        }

    }
}
