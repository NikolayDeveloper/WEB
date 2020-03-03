import { ExpertSearchModule } from './expert-search.module';

describe('ExpertSearchModule', () => {
  let expertSearchModule: ExpertSearchModule;

  beforeEach(() => {
    expertSearchModule = new ExpertSearchModule();
  });

  it('should create an instance', () => {
    expect(expertSearchModule).toBeTruthy();
  });
});
