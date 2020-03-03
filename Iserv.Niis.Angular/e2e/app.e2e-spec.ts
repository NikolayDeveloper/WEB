import { IservNiisAngularPage } from './app.po';

describe('iserv.niis.angular App', () => {
  let page: IservNiisAngularPage;

  beforeEach(() => {
    page = new IservNiisAngularPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
