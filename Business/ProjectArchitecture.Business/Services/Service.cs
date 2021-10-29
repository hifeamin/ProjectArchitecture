using System;
using System.Collections.Generic;
using System.Text;
using ProjectArchitecture.Business.Interfaces;
using ProjectArchitecture.Data.Repositories;
using ProjectArchitecture.Domain.Service;

namespace ProjectArchitecture.Business.Services {
    public class Service: IService {
        private readonly IDataRepository _repository;
        private readonly IValidationService _validationService;

        public Service(IDataRepository repository, IValidationService validationService) {
            _repository = repository;
            _validationService = validationService;
        }
    }
}
