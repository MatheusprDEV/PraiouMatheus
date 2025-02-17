using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PraiouMatheus.Data;
using PraiouMatheus.Models;
using System;
using System.Linq;

namespace PraiouMatheus.Controllers
{
    public class AvaliacaoController : Controller
    {
        private readonly AppCont _context;

        public AvaliacaoController(AppCont context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult EnviarAvaliacao(Avaliacao avaliacao)
        {
            avaliacao.UsuarioEmail = User.Identity.Name;
            var usuarioEmail = User.Identity.Name;
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
            avaliacao.Usuario = usuario;
            avaliacao.FK_UsuarioId = usuario.UsuarioId;

            // if (ModelState.IsValid)
            //{
                // Pega o e-mail do usuário logado (já armazenado nas claims do usuário)
                 usuarioEmail = User.Identity.Name;

                if (string.IsNullOrEmpty(usuarioEmail))
                {
                    ModelState.AddModelError("", "Usuário não autenticado.");
                    return View();
                }
                // Busca o usuário no banco de dados usando o e-mail
                 usuario = _context.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);

                if (usuario != null)
                {
                    // Define o ID do usuário para a avaliação
                    avaliacao.FK_UsuarioId = usuario.UsuarioId; // Assumindo que o campo de ID do usuário é UsuarioId
                    avaliacao.UsuarioEmail = usuario.Email; // Define o e-mail do usuário
                   

                    // Adiciona a avaliação ao contexto e salva no banco de dados
                    _context.Avaliacoes.Add(avaliacao);
                    _context.SaveChanges();

                    // Após salvar, redireciona para a página de visualização das avaliações
                    return RedirectToAction("VerAvaliacao", "Avaliacao");
                }
                else
                {
                    // Caso o usuário não seja encontrado no banco de dados (não deveria acontecer)
                    ModelState.AddModelError("", "Usuário não encontrado.");
                    return View();
                }
           // }

            // Caso o ModelState não seja válido, retorna à página com os dados inseridos
            return View();
        }

        [HttpGet]
        public IActionResult EnviarAvaliacao()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerAvaliacao()
        {
            // Recupera todas as avaliações do banco de dados
            var avaliacoes = _context.Avaliacoes.ToList();

            // Retorna a view com a lista de avaliações
            return View(avaliacoes);
        }

        [HttpPost]
        public IActionResult DeletarAvaliacao(int id)
        {
            // Localiza a avaliação pelo ID
            var avaliacao = _context.Avaliacoes.FirstOrDefault(a => a.AvaliacaoId == id);

            // Se a avaliação for encontrada, remove e salva as mudanças
            if (avaliacao != null)
            {
                _context.Avaliacoes.Remove(avaliacao);  // Remove a avaliação
                _context.SaveChanges();  // Salva as alterações no banco
            }

            // Redireciona para a página de visualização das avaliações após a exclusão
            return RedirectToAction("VerAvaliacao");
        }



    }
}
