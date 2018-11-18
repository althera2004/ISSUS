// --------------------------------
// <copyright file="EquipmentActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentActions : WebService
{

    public EquipmentActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(Equipment equipment, int userId, string scaleDivision, int companyId)
    {
        if (!string.IsNullOrEmpty(scaleDivision))
        {
            equipment.ScaleDivisionValue = Convert.ToDecimal(scaleDivision.Replace('.', ','));
        }

        return equipment.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update_(string data)
    {
        var x = new ActionResult();
        x.SetSuccess("1");
        return x;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Equipment newItem, Equipment oldItem, string scaleDivision, int userId, int companyId)
    {
        var res = ActionResult.NoAction;
        if(!string.IsNullOrEmpty(scaleDivision))
        {
            newItem.ScaleDivisionValue = Convert.ToDecimal(scaleDivision);//.Replace('.', ','));
        }

        string trace = newItem.Differences(oldItem);
        if (!string.IsNullOrEmpty(trace))
        {
            res = newItem.Update(userId, trace);
        }

        if(newItem.InternalCalibration == null)
		{
			newItem.InternalCalibration.Delete(userId);
		}
		else if (newItem.InternalCalibration.Id != 0 || oldItem.InternalCalibration == null)
        {
            if (newItem.InternalCalibration.Id == -1)
            {
                newItem.InternalCalibration.Insert(userId);
            }
        }
        else if (newItem.InternalCalibration.Id != 0 || oldItem.InternalCalibration.Id > 0)
        {
            if (newItem.InternalCalibration.Id == -1)
            {
                newItem.InternalCalibration.Insert(userId);
            }
            else
            {
                if (newItem.InternalCalibration.Id > 0)
                {
                    newItem.InternalCalibration.Update(userId);
                }
                else
                {
                    oldItem.InternalCalibration.Delete(userId);
                }
            }
        }

        if(newItem.ExternalCalibration == null)
		{
			newItem.ExternalCalibration.Delete(userId);
		}
		else if (newItem.ExternalCalibration.Id != 0 || oldItem.ExternalCalibration == null)
        {
            if (newItem.ExternalCalibration.Id == -1)
            {
                newItem.ExternalCalibration.Insert(userId);
            }
        }
        else if (newItem.ExternalCalibration.Id != 0 || oldItem.ExternalCalibration.Id > 0)
        {
            if (newItem.ExternalCalibration.Id == -1)
            {
                newItem.ExternalCalibration.Insert(userId);
            }
            else
            {
                if (newItem.ExternalCalibration.Id > 0)
                {
                    newItem.ExternalCalibration.Update(userId);
                }
                else
                {
                    oldItem.ExternalCalibration.Delete(userId);
                }
            }
        }

        if(newItem.InternalVerification == null)
		{
			newItem.InternalVerification.Delete(userId);
		}
		else if (newItem.InternalVerification.Id != 0 || oldItem.InternalVerification == null)
        {
            if (newItem.InternalVerification.Id == -1)
            {
                newItem.InternalVerification.Insert(userId);
            }
        }
        else if (newItem.InternalVerification.Id != 0 || oldItem.InternalVerification.Id > 0)
        {
            if (newItem.InternalVerification.Id == -1)
            {
                newItem.InternalVerification.Insert(userId);
            }
            else
            {
                if (newItem.InternalVerification.Id > 0)
                {
                    newItem.InternalVerification.Update(userId);
                }
                else
                {
                    oldItem.InternalVerification.Delete(userId);
                }
            }
        }
		
		if(newItem.ExternalVerification == null)
		{
			newItem.ExternalVerification.Delete(userId);
		}
		else if (newItem.ExternalVerification.Id != 0 || oldItem.ExternalVerification == null)
        {
            if (newItem.ExternalVerification.Id == -1)
            {
                newItem.ExternalVerification.Insert(userId);
            }
        }
        else if (newItem.ExternalVerification.Id != 0 || oldItem.ExternalVerification.Id > 0)
        {
            if (newItem.ExternalVerification.Id == -1)
            {
                newItem.ExternalVerification.Insert(userId);
            }
            else
            {
                if (newItem.ExternalVerification.Id > 0)
                {
                    newItem.ExternalVerification.Update(userId);
                }
                else
                {
                    oldItem.ExternalVerification.Delete(userId);
                }
            }
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentId, string reason, int userId, int companyId)
    {
        return new Equipment { Id = equipmentId, CompanyId = companyId }.Delete(userId, reason);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string GetFilter(long equipmentId, int companyId, bool calibrationInternal, bool calibrationExternal, bool verificationInternal, bool verificationExternal, bool maintenanceInternal, bool maintenanceExternal, bool repairInternal, bool repairExternal, DateTime? dateFrom, DateTime? dateTo)
    {
        return EquipmentRecord.EquipmentRecordJsonList(equipmentId, companyId, calibrationInternal, calibrationExternal, verificationInternal, verificationExternal, maintenanceInternal, maintenanceExternal, repairInternal, repairExternal, dateFrom, dateTo);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Anulate(int equipmentId, int companyId, int applicationUserId, string reason, DateTime date, int responsible)
    {
        return Equipment.Anulate(equipmentId, companyId, applicationUserId, reason, date, responsible);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Restore(int equipmentId, int companyId, int applicationUserId)
    {
        return Equipment.Restore(equipmentId, companyId, applicationUserId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public string SetFilter(string filter)
    {
        Session["EquipmentFilter"] = filter.ToUpperInvariant();
        return "OK";
    }
}