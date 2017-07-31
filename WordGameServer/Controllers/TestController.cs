using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace WordGameServer.Controllers
{
    public class TestController : ApiController
    {
        string[] products = new string[]
        {
            "test", "test2", "test3"
        };

        public IEnumerable<string> GetAllProducts()
        {
            return products;
        }

        public IHttpActionResult GetProduct(int id)
        {
            var product = products[id];
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
