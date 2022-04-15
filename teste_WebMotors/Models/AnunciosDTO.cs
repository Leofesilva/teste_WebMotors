using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace teste_WebMotors.Models
{
    [Table("tb_AnuncioWebMotors")]
    public class AnunciosDTO
    {
        [Column("Id")]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Column("marca")]
        [MaxLength(45, ErrorMessage = "Não é possível usar mais que 45 caracteres")]
        [Required(ErrorMessage = "Necessário Informar a Marca do Veículo")]
        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Column("modelo")]
        [MaxLength(45, ErrorMessage = "Não é possível usar mais que 45 caracteres")]
        [Required(ErrorMessage = "Necessário Informar o Modelo do Veículo")]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; }

        [Column("versao")]
        [MaxLength(45, ErrorMessage = "Não é possível usar mais que 45 caracteres")]
        [Required(ErrorMessage = "Necessário Informar a Versão do Veículo")]
        [Display(Name = "Versão")]
        public string Versao { get; set; }

        [Column("ano")]
        [Range(1960, 2022, ErrorMessage = "Informe o ano de Fabricação entre 1960 a 2022")]
        [Display(Name = "Ano")]
        [Required(ErrorMessage = "Necessário Informar o Ano de Fabricação do Veículo")]
        public int Ano { get; set; }

        [Column("quilometragem")]
        [Range(0, 999999, ErrorMessage = "Informe a quilometragem entre 0 e 999.999 ")]
        [Display(Name = "Quilometragem")]
        [Required(ErrorMessage = "Necessário Informar a Quilometragem do Veículo")]
        public int Quilometragem { get; set; }

        [Column("observacao")]
        [Display(Name = "Observação")]
        [Required(ErrorMessage = "Necessário colocar Alguma Observação sobre o Veículo")]
        public string Observacao { get; set; }

    }
}
