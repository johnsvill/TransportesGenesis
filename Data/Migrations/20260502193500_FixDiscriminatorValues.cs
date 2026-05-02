using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixDiscriminatorValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Actualizar todos los registros existentes con Discriminator vacío, NULL o inválido
            // Esto corrige el problema donde usuarios existentes tienen Discriminator = '' en lugar de 'AppUser'
            migrationBuilder.Sql(@"
                UPDATE AspNetUsers 
                SET Discriminator = 'AppUser' 
                WHERE Discriminator IS NULL 
                   OR Discriminator = '' 
                   OR LEN(RTRIM(Discriminator)) = 0
                   OR Discriminator NOT IN ('AppUser', 'IdentityUser');
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No revertir cambios de datos por seguridad
            // Los datos ya existentes se mantienen como 'AppUser'
        }
    }
}
