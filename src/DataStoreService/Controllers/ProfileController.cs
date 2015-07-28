using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using DataStoreService.Services;

namespace DataStoreService.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IStorageService _storageService;

        public ProfileController() : this(new FileStorageService()) { }

        public ProfileController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost]
        [Route("api/profile", Name = "ProfileCreate")]
        public IHttpActionResult CreateProfile(string key, string data)
        {
            //TODO: need to add idemptotent protection
            var profileId = _storageService.CreateProfile(key, data);
            var locationUrl = Url.Profile(profileId);
            var content = new { location = locationUrl };
            return Created(locationUrl, content);
        }

        [Route("api/profile/{id}", Name = "ProfileGet")]
        public IHttpActionResult GetProfile(string id)
        {
            try
            {
                var jsonData = _storageService.GetProfile(id);
                return Ok<object>(jsonData);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        public IHttpActionResult SetProfile(string id, string data)
        {
            try
            {
                _storageService.SetProfile(id, data);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
