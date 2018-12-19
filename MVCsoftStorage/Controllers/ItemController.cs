﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCsoftStorage.Models;

namespace MVCsoftStorage.Controllers
{
    public class ItemController : Controller
    {
        public ActionResult Program(int? id)
        {
            DBContext db = new DBContext();

            int currentId = id ?? 0;

            List<int> postsId = (from el in db.posts
                                 select el.id).ToList();

            if (!postsId.Contains(currentId))
                return View("Error");

            var query =
                from el in db.posts
                where el.id == currentId && el.visible == 1 && el.date_public < DateTime.Now
                select new ProgramModel
                {
                    Name = el.name,
                    Desc = el.description,
                    Spoilers = el.spoilers.ToList(),
                    Imgs = el.images.Where(n => n.type == "screen").ToList(),
                    Program = el.programs,
                    DatePublic = el.date_public ?? DateTime.Now,
                };

            ViewBag.Post = query.SingleOrDefault();

            return View();
        }

        public FileResult DownloadFile()
        {
            string fullPath = @"~/static/torrent/sims2.torrent";
            return File(fullPath, "application/torrent", "sims.torrent");
        }
    }
}