﻿using Diploma.DataAccess.Data;
using Diploma.DataAccess.Repository.IRepository;
using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Diploma.DataAccess.Repository.CompanyRepository;

namespace Diploma.DataAccess.Repository
{
	public class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		private ApplicationDbContext _db;
		public CompanyRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Company obj)
		{
			_db.Companies.Update(obj);
		}
	}
}
