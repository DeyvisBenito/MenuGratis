﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuGratis.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult ModUsuario()
        {
            return View();
        }

        public ActionResult ModRestaurante() 
        {
            return View();
        }

    }
}