using Transport.Model;
using Transport.Repository;

namespace Transport.Service
{
    class ClientService : AbstractService<int, Client>
    {
        private readonly ClientDBRepository _clientDBRepository;

        public ClientService(IRepository<int, Client> repository) : base(repository)
        {
            _clientDBRepository = (ClientDBRepository)repository;
        }

        public int FilterClientById(string firstName, string lastName)
        {
            return _clientDBRepository.FindByFirstAndLastName(firstName, lastName).Id;
        }
    }
}
