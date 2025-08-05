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

  logins: LoginInfo[];
  login: LoginInfo;
  newUserDialogVisible = false;
  roles = Object.keys(Roles).filter(k => +k + 1);

  constructor(private adminService: AdminService,
              private confirmationService: ConfirmationService,
              private messageService: MessageService,
              private auth: AuthService) {
  }

  ngOnInit(): void {
    this.adminService.getUserList().subscribe(logins => {
      this.logins = logins;
    });
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
}
