﻿using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer
{
    public class DALMission
    {
        private readonly AppDbContext _cIDbContext;

        public DALMission(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        public  List<Missions> MissionList()
        {
            return _cIDbContext.Missions.ToList();
        }

        public string AddMission(Missions mission)
        {
            string result = "";
            try
            {
                _cIDbContext.Missions.Add(mission);
                _cIDbContext.SaveChanges();
                result = "Mission added Successfully.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public List<Missions> ClientSideMissionList(int userId)
        {
            List<Missions> clientSideMissionList = new List<Missions>();
            try
            {
                clientSideMissionList = _cIDbContext.Missions
                    .Where(m => !m.IsDeleted)
                    .OrderBy(m => m.CreatedDate)
                    .Select(m => new Missions
                    {
                        Id = m.Id,
                        CountryId = m.CountryId,
                        CountryName = m.CountryName,
                        CityId = m.CityId,
                        CityName = m.CityName,
                        MissionTitle = m.MissionTitle,
                        MissionDescription = m.MissionDescription,
                        MissionOrganisationName = m.MissionOrganisationName,
                        MissionOrganisationDetail = m.MissionOrganisationDetail,
                        TotalSheets = m.TotalSheets,
                        RegistrationDeadLine = m.RegistrationDeadLine,
                        MissionThemeId = m.MissionThemeId,
                        MissionThemeName = m.MissionThemeName,
                        MissionImages = m.MissionImages,
                        MissionDocuments = m.MissionDocuments,
                        MissionSkillId = m.MissionSkillId,
                        MissionSkillName = string.Join(",", m.MissionSkillName),
                        MissionAvilability = m.MissionAvilability,
                        MissionVideoUrl = m.MissionVideoUrl,
                        MissionType = m.MissionType,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate,
                        MissionStatus = m.RegistrationDeadLine < DateTime.Now.AddDays(-1) ? "Closed" : "Available",
                        MissionApplyStatus = _cIDbContext.MissionApplication.Any(ma => ma.MissionId == m.Id && ma.UserId == userId) ? "Applied" : "Apply",
                        MissionApproveStatus = _cIDbContext.MissionApplication.Any(ma => ma.MissionId == m.Id && ma.UserId == userId && ma.Status == true) ? "Approved" : "Applied",
                        MissionDateStatus = m.EndDate <= DateTime.Now.AddDays(-1) ? "MissionEnd" : "MissionRunning",
                        MissionDeadLineStatus = m.RegistrationDeadLine <= DateTime.Now.AddDays(-1) ? "Closed" : "Running",
                        MissionFavouriteStatus = _cIDbContext.MissionFavourites.Any(mf => mf.MissionId == m.Id && mf.UserId == userId) ? "1" : "0",
                        Rating = _cIDbContext.MissionRating.FirstOrDefault(mr => mr.MissionId == m.Id && mr.UserId == userId).Rating ?? 0
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return clientSideMissionList;
        }
    }
}
