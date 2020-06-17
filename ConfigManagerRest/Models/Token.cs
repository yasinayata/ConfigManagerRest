using ConfigManagerRest.Encryption;
using ConfigManagerRest.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigManagerRest.Models
{    
    public class Token
    {
        #region UserLogin
        internal OperationResult<SessionUser> UserLogin(User user)
        {
            OperationResult<SessionUser> opu = new OperationResult<SessionUser>();
            OperationResult op = new OperationResult();
            SessionUser sessionUser = new SessionUser();
            string textToken = "";

            if (string.IsNullOrEmpty(user.Username))
            {
                opu.Result = false;
                opu.Message = "Invalid User";

                return opu;
            }

            try
            {
                //you could insert your own user check process...

                user.SessionId = Guid.NewGuid();
                user.Guid = Guid.NewGuid();

                int ActiveDays = 1;     //Token validity period
                DateTime ExpirationDateTime = DateTime.UtcNow.AddDays(ActiveDays);

                TokenUser tokenUser = new TokenUser { Guid = user.Guid, Username = user.Username, Password = "", SessionId = user.SessionId, ExpirationDateTime = ExpirationDateTime };

                textToken = tokenUser.Serialize();

                op = SymmetricalEncryption.DESEncryptedLimitedTime(textToken, ExpirationDateTime);

                user.Token = op.Message;

                sessionUser.SessionId = user.SessionId;
                sessionUser.Token = user.Token;

                opu.Data = sessionUser;
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.ToString();
            }

            return opu;
        }
        #endregion

        #region UserCheck   - This method will be internal operation, each methods [GET / PUT / DELETE etch] will check user information firstly
        internal OperationResult UserCheck(SessionUser sessionUser)
        {
            OperationResult op = new OperationResult();
            OperationResult opt = new OperationResult();

            TokenUser tokenUser = new TokenUser();
            string textToken;
            try
            {
                textToken = sessionUser.Token;


                opt = SymmetricalEncryption.DESDecryptedLimitedTime(textToken);
                if (!opt.Result)
                {
                    //Invalid token / Token expired / etc
                    op.Result = opt.Result;
                    op.Message = $"Invalid Token : {opt.Message}";

                    return op;
                }

                op = new OperationResult();

                textToken = opt.Message;
                tokenUser = textToken.Deserialize<TokenUser>();

                if (sessionUser.SessionId != tokenUser.SessionId)
                {
                    //Token . SessionId / SessionUser . SessionId is different
                    op.Result = false;
                    op.Message = $"Unknown User - SessionId : {sessionUser.SessionId}";
                    return op;
                }
            }
            catch (Exception exception)
            {
                op.Result = false;
                op.Message = exception.Message;
            }

            return op;
        }
        #endregion
    }

}