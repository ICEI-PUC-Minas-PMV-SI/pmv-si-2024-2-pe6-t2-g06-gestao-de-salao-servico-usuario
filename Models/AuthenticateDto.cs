﻿using System.ComponentModel.DataAnnotations;

namespace pmv_si_2024_2_pe6_t2_g06_gestao_de_salao_servico_usuario.Models
{
    public class AuthenticateDto
    {
        [Required]
        public int Id { get; set; }


        [Required]
        public string Senha {  get; set; }

    }
}
