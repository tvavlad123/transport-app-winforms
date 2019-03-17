using System;
using System.Collections.Generic;
using System.Linq;
using Transport.Repository;

namespace Transport.Service
{
    class EmployeeService
    {
        private readonly EmployeeDBRepository _employeeDBRepository;

        public EmployeeService(EmployeeDBRepository employeeDBRepository)
        {
            _employeeDBRepository = employeeDBRepository;
        }

        public bool Login(string username, string password)
        {
            return _employeeDBRepository.FindByCredentials(username, password);
        }
    }
}
