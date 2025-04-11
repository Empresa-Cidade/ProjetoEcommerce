using Microsoft.AspNetCore.Mvc;
using ProjetoEcommerce.Models;
using ProjetoEcommerce.Repositorio;

namespace ProjetoEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _ProdutoRepositorio;
        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _ProdutoRepositorio = produtoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_ProdutoRepositorio.TodosProdutos());
        }

        public IActionResult CadastrarProduto()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarProduto(Produto produto)
        {
            _ProdutoRepositorio.Cadastrar(produto);
            return RedirectToAction(nameof(Index));
        }

        
        public IActionResult EditarProduto(int id)
        {
            var produto = _ProdutoRepositorio.ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult EditarProduto(int id, [Bind("CodProd, NomeProd,Descricao,Quantidade,Preco")]Produto produto)
        {
            if (id != produto.CodProd)
            {
                return BadRequest(); 
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (_ProdutoRepositorio.Atualizar(produto))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocorreu um erro ao Editar.");
                    return View(produto);
                }
            }
            return View(produto);
        }


        public IActionResult ExcluirProduto(int id)
        {
            _ProdutoRepositorio.ExcluirProduto(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

