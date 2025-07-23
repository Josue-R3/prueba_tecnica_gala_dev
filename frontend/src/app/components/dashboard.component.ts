import { Component, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faUser,
  faUsers,
  faMapMarkerAlt,
  faShield,
  faUserCheck,
  faSignOutAlt,
  faPlus,
  faTrash,
  faTimes,
  faEye,
  faEyeSlash,
  faStore,
  faUserPlus,
  faShieldAlt,
} from '@fortawesome/free-solid-svg-icons';
import { AuthService } from '../services/auth.service';
import {
  DataService,
  Tienda,
  Empleado,
  Usuario,
  Role,
} from '../services/data.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, FontAwesomeModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  // Font Awesome icons
  faUser = faUser;
  faUsers = faUsers;
  faMapMarkerAlt = faMapMarkerAlt;
  faShield = faShield;
  faUserCheck = faUserCheck;
  faSignOutAlt = faSignOutAlt;
  faPlus = faPlus;
  faTrash = faTrash;
  faTimes = faTimes;
  faEye = faEye;
  faEyeSlash = faEyeSlash;
  faStore = faStore;
  faUserPlus = faUserPlus;
  faShieldAlt = faShieldAlt;

  activeTab = signal<'tiendas' | 'empleados' | 'usuarios' | 'roles'>('tiendas');

  // Estados para modales
  isTiendaModalOpen = signal(false);
  isEmpleadoModalOpen = signal(false);
  isUsuarioModalOpen = signal(false);
  isRoleModalOpen = signal(false);
  showLogoutConfirm = signal(false);

  // Estados para formularios
  newTienda = signal({
    nombre: '',
    direccion: '',
    estado: 1,
  });

  newEmpleado = signal({
    nombre: '',
    apellido: '',
    correo: '',
    cargo: '',
    tiendaId: 0,
    estado: 1,
  });

  newUsuario = signal({
    usuario: '',
    contrasenia: '',
    rolId: 1,
    empleadoId: null as number | null,
    estado: 1,
  });

  newRole = signal({
    nombre: '',
  });

  constructor(
    private authService: AuthService,
    private dataService: DataService,
    private router: Router
  ) {}

  // Computed properties que se evalúan después de la construcción
  get currentUser() {
    return this.authService.currentUser;
  }

  get estadisticas() {
    return computed(() => this.dataService.getEstadisticas());
  }

  get roles() {
    return this.dataService.roles;
  }

  get tiendas() {
    return this.dataService.tiendas;
  }

  get empleados() {
    return this.dataService.empleados;
  }

  get usuarios() {
    return this.dataService.usuarios;
  }

  ngOnInit() {
    // Si no está autenticado, redirigir al login
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
    }
  }

  getCurrentUserDisplayName(): string {
    const user = this.currentUser();
    if (user?.empleado) {
      return `${user.empleado.nombre} ${user.empleado.apellido}`;
    }
    return user?.usuario || '';
  }

  setActiveTab(tab: 'tiendas' | 'empleados' | 'usuarios' | 'roles') {
    this.activeTab.set(tab);
  }

  // Funciones para obtener nombres
  getTiendaNombre(tiendaId: number): string {
    return this.dataService.getTiendaNombre(tiendaId);
  }

  getRolNombre(rolId: number): string {
    return this.dataService.getRolNombre(rolId);
  }

  // Métodos auxiliares para filtros
  getEmpleadosCountByTienda(tiendaId: number): number {
    return this.empleados().filter(
      (e) => e.tiendaId === tiendaId && e.estado === 1
    ).length;
  }

  getUsuariosCountByRole(roleId: number): number {
    return this.usuarios().filter((u) => u.rolId === roleId && u.estado === 1)
      .length;
  }

  getEmpleadoNombre(empleadoId: number | null): string {
    return this.dataService.getEmpleadoNombre(empleadoId);
  }

  // Métodos para manejar cambios en formularios
  updateTiendaNombre(nombre: string) {
    this.newTienda.update((t) => ({ ...t, nombre }));
  }

  updateTiendaDireccion(direccion: string) {
    this.newTienda.update((t) => ({ ...t, direccion }));
  }

  updateEmpleadoNombre(nombre: string) {
    this.newEmpleado.update((e) => ({ ...e, nombre }));
  }

  updateEmpleadoApellido(apellido: string) {
    this.newEmpleado.update((e) => ({ ...e, apellido }));
  }

  updateEmpleadoCorreo(correo: string) {
    this.newEmpleado.update((e) => ({ ...e, correo }));
  }

  updateEmpleadoCargo(cargo: string) {
    this.newEmpleado.update((e) => ({ ...e, cargo }));
  }

  updateEmpleadoTienda(tiendaId: string) {
    this.newEmpleado.update((e) => ({ ...e, tiendaId: +tiendaId }));
  }

  updateUsuarioNombre(usuario: string) {
    this.newUsuario.update((u) => ({ ...u, usuario }));
  }

  updateUsuarioContrasenia(contrasenia: string) {
    this.newUsuario.update((u) => ({ ...u, contrasenia }));
  }

  updateUsuarioRol(rolId: string) {
    this.newUsuario.update((u) => ({ ...u, rolId: +rolId }));
  }

  updateUsuarioEmpleado(empleadoId: string | null) {
    this.newUsuario.update((u) => ({
      ...u,
      empleadoId: empleadoId ? +empleadoId : null,
    }));
  }

  updateRoleNombre(nombre: string) {
    this.newRole.update((r) => ({ ...r, nombre }));
  }

  // Funciones para manejar formularios
  handleAddTienda() {
    const tienda = this.newTienda();
    if (tienda.nombre && tienda.direccion) {
      this.dataService.addTienda(tienda);
      this.newTienda.set({ nombre: '', direccion: '', estado: 1 });
      this.isTiendaModalOpen.set(false);
    }
  }

  handleAddEmpleado() {
    const empleado = this.newEmpleado();
    if (
      empleado.nombre &&
      empleado.apellido &&
      empleado.correo &&
      empleado.cargo &&
      empleado.tiendaId
    ) {
      this.dataService.addEmpleado(empleado);
      this.newEmpleado.set({
        nombre: '',
        apellido: '',
        correo: '',
        cargo: '',
        tiendaId: 0,
        estado: 1,
      });
      this.isEmpleadoModalOpen.set(false);
    }
  }

  handleAddUsuario() {
    const usuario = this.newUsuario();
    if (usuario.usuario && usuario.contrasenia && usuario.rolId) {
      this.dataService.addUsuario({
        usuario: usuario.usuario,
        rolId: usuario.rolId,
        empleadoId: usuario.empleadoId,
        estado: usuario.estado,
      });
      this.newUsuario.set({
        usuario: '',
        contrasenia: '',
        rolId: 1,
        empleadoId: null,
        estado: 1,
      });
      this.isUsuarioModalOpen.set(false);
    }
  }

  handleAddRole() {
    const role = this.newRole();
    if (role.nombre) {
      this.dataService.addRole(role);
      this.newRole.set({ nombre: '' });
      this.isRoleModalOpen.set(false);
    }
  }

  // Funciones para "eliminar" (cambiar estado a 0)
  handleDeleteTienda(id: number) {
    this.dataService.deleteTienda(id);
  }

  handleDeleteEmpleado(id: number) {
    this.dataService.deleteEmpleado(id);
  }

  handleDeleteUsuario(id: number) {
    this.dataService.deleteUsuario(id);
  }

  // Función para cerrar sesión
  handleLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Función para verificar permisos
  canManage(section: string): boolean {
    const userRole = this.currentUser()?.rol?.nombre;

    switch (section) {
      case 'tiendas':
      case 'usuarios':
      case 'roles':
        return userRole === 'Admin';
      case 'empleados':
        return userRole === 'Admin' || userRole === 'Manager';
      default:
        return false;
    }
  }

  // Función para filtrar datos activos
  getActiveTiendas() {
    return this.tiendas().filter((t) => t.estado === 1);
  }

  getActiveEmpleados() {
    return this.empleados().filter((e) => e.estado === 1);
  }

  getActiveUsuarios() {
    return this.usuarios().filter((u) => u.estado === 1);
  }

  getActiveTiendasForSelect() {
    return this.tiendas().filter((t) => t.estado === 1);
  }

  getActiveEmpleadosForSelect() {
    return this.empleados().filter((e) => e.estado === 1);
  }
}
