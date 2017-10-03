using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cmcglynn_blog.Models;
using Microsoft.AspNet.Identity;

namespace cmcglynn_blog.Controllers
{
    [RequireHttps]
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Comments
        //public ActionResult Index()
        //{
        //    var comments = db.Comments.Include(c => c.Author).Include(c => c.BlogPost);
        //    return View(comments.ToList());
        //}

        //// GET: Comments/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Comments comments = db.Comments.Find(id);
        //    if (comments == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(comments);
        //}

        //// GET: Comments/Create
        //[Authorize]
        //public ActionResult Create()
        //{
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
        //    ViewBag.PostId = new SelectList(db.Posts, "Id", "MediaUrl");
        //    return View();
        //}

        // POST: Comments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostId,Body")] Comments comment)
        {
            Post post = db.Posts.Find(comment.PostId);

            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                comment.AuthorId = user.Id;
                comment.Created = DateTime.Now;
                db.Comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { slug = post.Slug });
            }

            return RedirectToAction("Details", "Posts", new { slug = post.Slug });
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostId,AuthorId,Body,Created,Updated,UpdateReason")] Comments comment)
        {
            Post post = db.Posts.Find(comment.PostId);

            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                comment.Updated = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Details", "Posts", new { slug = post.Slug });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comment = db.Comments.Find(id);
            Post post = db.Posts.Find(comment.PostId);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Details", "Posts", new { slug = post.Slug });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
