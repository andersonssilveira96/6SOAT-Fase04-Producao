using Application.UseCase.Pedidos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducaoController : ControllerBase
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        public ProducaoController(IPedidoUseCase pedidoUseCase)
        {
            _pedidoUseCase = pedidoUseCase;
        }     
       
        [HttpPut]
        [Route("atualizar-status/{id}/{status}")]
        public async Task<IActionResult> AtualizarStatus(long id, int status)
        {
            try
            {
                return Ok(await _pedidoUseCase.AtualizarStatus(id, status));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return Ok(await _pedidoUseCase.Listar());
        }
    }
}
