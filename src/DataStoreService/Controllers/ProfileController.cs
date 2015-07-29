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
                SignedEnvelope signedEnvelope = JsonConvert.DeserializeObject<SignedEnvelope>(data);

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
