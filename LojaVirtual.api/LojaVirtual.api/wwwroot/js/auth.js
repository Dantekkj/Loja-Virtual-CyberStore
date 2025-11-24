// Script compartilhado para gerenciar autenticação
const AuthManager = {
    API_URL: window.location.origin,

    // Obter token do localStorage
    getToken() {
        return localStorage.getItem('token');
    },

    // Obter usuário do localStorage
    getUsuario() {
        const usuarioStr = localStorage.getItem('usuario');
        return usuarioStr ? JSON.parse(usuarioStr) : null;
    },

    // Verificar se está logado
    isLoggedIn() {
        return !!this.getToken();
    },

    // Salvar URL de retorno antes de ir para login
    saveReturnUrl() {
        const currentPage = window.location.pathname.split('/').pop() || 'index.html';
        // Não salvar se já estiver em Login.html ou Cadastro.html
        if (currentPage !== 'Login.html' && currentPage !== 'Cadastro.html') {
            sessionStorage.setItem('returnUrl', currentPage);
        }
    },

    // Obter URL de retorno
    getReturnUrl() {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get('return') || sessionStorage.getItem('returnUrl') || 'index.html';
    },

    // Limpar URL de retorno
    clearReturnUrl() {
        sessionStorage.removeItem('returnUrl');
    },

    // Redirecionar para login salvando a página atual
    redirectToLogin() {
        this.saveReturnUrl();
        window.location.href = 'Login.html';
    },

    // Fazer logout
    logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('usuario');
        window.location.href = 'index.html';
    },

    // Atualizar header com informações do usuário
    updateHeader() {
        const usuario = this.getUsuario();
        const loginButton = document.getElementById('loginButton');
        const userMenu = document.getElementById('userMenu');
        const userAvatar = document.getElementById('userAvatar');
        const userName = document.getElementById('userName');
        const logoutButton = document.getElementById('logoutButton');

        if (usuario && this.isLoggedIn()) {
            // Usuário logado - mostrar foto e menu
            if (loginButton) loginButton.classList.add('hidden');
            if (userMenu) userMenu.classList.remove('hidden');
            if (userAvatar) {
                userAvatar.src = usuario.fotoUrl || 'https://www.gravatar.com/avatar/00000000000000000000000000000000?d=identicon&s=200';
                userAvatar.alt = usuario.nome || 'Usuário';
            }
            if (userName) userName.textContent = usuario.nome || usuario.email;
        } else {
            // Usuário não logado - mostrar botão de login
            if (loginButton) loginButton.classList.remove('hidden');
            if (userMenu) userMenu.classList.add('hidden');
        }

        // Configurar evento de logout
        if (logoutButton) {
            logoutButton.addEventListener('click', (e) => {
                e.preventDefault();
                this.logout();
            });
        }
    },

    // Obter headers com autenticação
    getAuthHeaders() {
        const token = this.getToken();
        return {
            'Content-Type': 'application/json',
            ...(token && { 'Authorization': `Bearer ${token}` })
        };
    }
};

// Atualizar header quando a página carregar
document.addEventListener('DOMContentLoaded', () => {
    AuthManager.updateHeader();
    
    // Adicionar evento para salvar URL de retorno quando clicar em links de login
    document.querySelectorAll('a[href*="Login.html"]').forEach(link => {
        link.addEventListener('click', (e) => {
            AuthManager.saveReturnUrl();
        });
    });
    
    // Adicionar evento para salvar URL de retorno quando clicar em links de cadastro
    document.querySelectorAll('a[href*="Cadastro.html"]').forEach(link => {
        link.addEventListener('click', (e) => {
            // Se estiver vindo do login, preservar o returnUrl
            const returnUrl = AuthManager.getReturnUrl();
            if (returnUrl !== 'index.html') {
                sessionStorage.setItem('returnUrl', returnUrl);
            }
        });
    });
});

