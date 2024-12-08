using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHome.Application.Features.Dtos;
public record PetInfoDto(PetMainInfoDto PetMainInfoDto, List<PhotoDto> PhotoDetailsDto);
