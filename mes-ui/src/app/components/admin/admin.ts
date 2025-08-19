import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {AdminService} from '../../services/admin-service';
import {LoginInfo} from '../../services/model/LoginInfo';
import {Roles} from '../../enums/roles';
import {Button} from 'primeng/button';
import {Dialog} from 'primeng/dialog';
import {AutoFocus} from 'primeng/autofocus';
import {ConfirmationService, MessageService} from 'primeng/api';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {AuthService} from '../../services/auth-service';
import {CarExecutionDto} from '../../Entities/CarExecutionDto';
import {DirectoryService} from '../../services/directory-service';


@Component({
  selector: 'app-admin',
  imports: [
    Button,
    Dialog,
    AutoFocus,
    ConfirmDialog
  ],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class Admin implements OnInit {

  @ViewChild('loginName') loginName: ElementRef;
  @ViewChild('role') role: ElementRef;
  @ViewChild('password') password: ElementRef;
  @ViewChild('carExecutionCode') carExecutionCode: ElementRef;
  @ViewChild('carExecutionName') carExecutionName: ElementRef;

  logins: LoginInfo[];
  carExecutions: CarExecutionDto[];
  login: LoginInfo;
  carExecution: CarExecutionDto;
  newUserDialogVisible = false;
  newCarExecutionVisible = false;
  changeRoleDialogVisible = false;
  changeCarExecutionVisible = false;
  changePasswordDialogVisible = false;
  roles = Object.keys(Roles).filter(k => +k + 1);

  constructor(private adminService: AdminService,
              private directoryService: DirectoryService,
              private confirmationService: ConfirmationService,
              private messageService: MessageService,
              protected auth: AuthService) {
  }

  ngOnInit(): void {
    this.adminService.getUserList().subscribe(logins => {
      this.logins = logins;
    });

    this.directoryService.getCarExecutionList().subscribe(carExecutions => {
      this.carExecutions = carExecutions;
    })
  }

  addUser(newUser: LoginInfo) {
    this.adminService.addUser(newUser).subscribe(user => {
      this.logins.push(user);
      this.newUserDialogVisible = false;
      this.loginName.nativeElement.value = '';
      this.role.nativeElement.value = '';
      this.password.nativeElement.value = '';
    })
  }

  deleteUser(login: LoginInfo): void {
    this.login = login;
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Вы действительно хотите удалить учетную запись "${login.name}"?`,
      icon: 'pi pi-exclamation-circle',
      acceptButtonProps: {
        label: 'Save',
        icon: 'pi pi-check',
        size: 'small'
      },
      rejectButtonProps: {
        label: 'Cancel',
        icon: 'pi pi-times',
        variant: 'outlined',
        size: 'small'
      },
      accept: () => {
        const currentName = this.auth.name;
        if (login.name === currentName) {
          this.messageService.add({
            severity: 'warn',
            summary: 'Warning',
            detail: 'Невозможно удалить текущего пользователя.',
            life: 3000
          });
          return;
        }

        this.adminService.deleteUser(login.name).subscribe(data => {
          this.logins = this.logins.filter(login => login.name !== data['name']);
        })
        this.messageService.add({
          severity: 'info',
          summary: 'Удаление',
          detail: `Учетная запись "${login.name}" удалена.`,
          life: 3000
        });
      },
      reject: () => {
        this.messageService.add({severity: 'error', summary: 'Отмена', detail: 'Удаление отменено.', life: 3000});
      }
    });
  }

  protected readonly Roles = Roles;
  protected readonly Object = Object;

  openChangeRoleDialog(login: LoginInfo) {
    this.login = login;
    this.changeRoleDialogVisible = true;
  }

  changeRole(loginName: string, role: number) {
    this.adminService.changeRole({name: this.login.name, role: this.login.role, password: ''}, role).subscribe(data => {
      const logName = this.logins.find(login => login.name === loginName);
      if (logName) {
        logName.role = role;
      }
      this.login = null;
      this.changeRoleDialogVisible = false;
      this.messageService.add({severity: 'success', summary: 'Update', detail: 'Роль успешно изменена.'});
    })
  }

  openChangePasswordDialog(login: LoginInfo) {
    this.login = login;
    this.changePasswordDialogVisible = true;
  }

  changePassword(oldPassword: string, newPassword: string) {
    this.adminService.changePassword({
      name: this.login.name,
      role: 0,
      password: oldPassword
    }, newPassword).subscribe(data => {
      this.login = null;
      this.changePasswordDialogVisible = false;
      this.messageService.add({severity: 'success', summary: 'Change', detail: 'Пароль успешно изменен.'});
    }, error => {
      console.log(error)
      this.messageService.add({
        severity: 'error',
        summary: 'Отмена',
        detail: error.error.errors[0].description,
        life: 3000
      });
    })
  }

  addCarExecution(carExecution: CarExecutionDto) {
    this.directoryService.addCarExecution(carExecution).subscribe(carExecution => {
      this.carExecutions.push(carExecution);
      this.newCarExecutionVisible = false;
      this.carExecutionCode.nativeElement.value = '';
      this.carExecutionName.nativeElement.value = '';
    })
  }

  deleteCarExecution(carExecutionDto: CarExecutionDto): void {
    this.carExecution = carExecutionDto;
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Вы действительно хотите удалить учетную запись "${carExecutionDto.code}: ${carExecutionDto.name}"?`,
      icon: 'pi pi-exclamation-circle',
      acceptButtonProps: {
        label: 'Save',
        icon: 'pi pi-check',
        size: 'small'
      },
      rejectButtonProps: {
        label: 'Cancel',
        icon: 'pi pi-times',
        variant: 'outlined',
        size: 'small'
      },
      accept: () => {
        this.directoryService.deleteCarExecution(carExecutionDto.id).subscribe(data => {
          this.carExecutions = this.carExecutions.filter(ce => ce.code.trim() !== this.carExecution.code.trim());
        })
        this.messageService.add({
          severity: 'info',
          summary: 'Удаление',
          detail: `Надстройка "${carExecutionDto.code}: ${carExecutionDto.name}" удалена.`,
          life: 3000
        });
      },
      reject: () => {
        this.messageService.add({severity: 'error', summary: 'Отмена', detail: 'Удаление отменено.', life: 3000});
      }
    });
  }

  openChangeCarExecutionDialog(carExecutionDto: CarExecutionDto) {
    this.carExecution = carExecutionDto;
    this.changeCarExecutionVisible = true;
  }

  changeCarExecution(code: string, name: string) {
    this.directoryService.updateCarExecution({code, name, id: this.carExecution.id}).subscribe(data => {
      const carExecution = this.carExecutions.find(ce => ce.id === this.carExecution.id);
      if (carExecution) {
        carExecution.code = code;
        carExecution.name = name;
      }
      this.carExecution = null;
      this.changeCarExecutionVisible = false;
      this.messageService.add({severity: 'success', summary: 'Update', detail: 'Тип надстройки успешно изменен.'});
    })
  }
}
