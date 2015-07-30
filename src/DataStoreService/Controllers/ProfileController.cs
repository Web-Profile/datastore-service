using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using DataStoreService.Services;
using Newtonsoft.Json;
using System.Security.Cryptography;

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
        public IHttpActionResult CreateProfile([FromBody] Profile profile)
        {

            //TODO: need to add idemptotent protection
            var profileId = _storageService.CreateProfile(profile);
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

                Profile profile = new Profile();
                profile.Id = id;
                profile.Data = jsonData;

                return Ok(profile);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("api/profile/{id}", Name="ProfileSet")]
        public IHttpActionResult SetProfile(string id, [FromBody] SignedEnvelope signedEnvelope)
        {
            try
            {

                if(VerySignatureOfSignedEnvelope(id, signedEnvelope) == false)
                {
                    return BadRequest();

                }

                _storageService.SetProfile(id, signedEnvelope.Data);
                return Ok();
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        #region Validator
        public bool VerySignatureOfSignedEnvelope(string id, SignedEnvelope signedEnvelope)
        {
            bool ret = false;

            string profile = _storageService.GetProfile(id);

            if (String.IsNullOrEmpty(profile) == false)
            {
                //TODO: Look into profile and search for public key
                string key = signedEnvelope.AgentId;  ///HACK: should look into profile and grab public key of UserAget Service

                if (CryptographyHelper.VerifySignedData(signedEnvelope.Data, signedEnvelope.Signature, key) == true)
                {
                    ret = true;
                }
            }

            return ret;
        }
        #endregion
    }
}
