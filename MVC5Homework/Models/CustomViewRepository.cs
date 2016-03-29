using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5Homework.Models
{   
	public  class CustomViewRepository : EFRepository<CustomView>, ICustomViewRepository
	{

	}

	public  interface ICustomViewRepository : IRepository<CustomView>
	{

	}
}